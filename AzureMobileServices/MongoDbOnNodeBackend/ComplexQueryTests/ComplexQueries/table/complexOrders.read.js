function read(query, user, request) {
    var collectionName = tables.current.getTableName();
    var mongoHelper = require('../shared/mongoHelper');
    mongoHelper.connectAndGetCollection(collectionName, function(err, collection) {
        if (err) {
            console.log('Error creating collection: ', err);
            request.respond(500, { error: err });
            return;
        }

        if (query.id) {
            findSingleObject(collection, query.id, mongoHelper, request);
        } else {
            returnMultipleObjects(collection, query, mongoHelper, request);
        }
    });
}

function findSingleObject(collection, itemId, mongoHelper, request) {
    // Lookup operation: get for a single element
    collection.findOne(mongoHelper.queryForId(itemId), function(err, item) {
        if (err) {
            console.log('error querying collection: ', err);
            request.respond(500, { error: err });
        } else {
            if (item) {
                mongoHelper.mongoIdToMobileServiceId(item);
                request.respond(200, item);
            } else {
                request.respond(404);
            }
        }
    });
}

function returnMultipleObjects(collection, query, mongoHelper, request) {
    var findOptions = {};
    var findQuery = {};
    
    var queryComponents = query.getComponents();
    console.log('Query components: ', queryComponents);

    applyTopAndSkip(findOptions, queryComponents);
    applySelect(findOptions, queryComponents);
    applyOrdering(findOptions, queryComponents);
    var findQuery = applyFilter(queryComponents, request);
    if (findQuery === null) {
        // response already sent
        return;
    }

    collection.find(findQuery, findOptions).toArray(function(err, items) {
        if (err) {
            console.log('error querying collection: ', err);
            request.respond(200, { error: err });
        } else {
            items.forEach(function(item) {
                mongoHelper.mongoIdToMobileServiceId(item);
            });
            request.respond(200, items);
        }
    });

    function applyTopAndSkip(findOptions, queryComponents) {
        if (queryComponents.take) {
            findOptions.limit = queryComponents.take;
        }

        if (queryComponents.skip) {
            findOptions.skip = queryComponents.skip;
        }
    }

    function applySelect(findOptions, queryComponents) {
        var selects = queryComponents.selections;
        if (selects && selects.length) {
            if (selects.length === 1 && selects[0] === '*') {
                // Same as no $select, nothing to do
            } else {
                findOptions.fields = {};
                selects.forEach(function(field) {
                    findOptions.fields[field] = 1;
                });
            }
        }
    }

    function applyOrdering(findOptions, queryComponents) {
        var orderBy = [];
        var ordering = queryComponents.ordering;
        for (var orderField in ordering) {
            if (ordering.hasOwnProperty(orderField)) {
                var ascending = queryComponents.ordering[orderField] ? 'ascending' : 'descending';
                orderBy.push([ orderField, ascending ]);
            }
        }

        if (orderBy.length) {
            findOptions.sort = orderBy;
        }
    }

    function applyFilter(queryComponents, request) {
        // Simple case: filter that excludes everything; no need to talk to the DB
        if (queryComponents.filters && queryComponents.filters.queryString === 'false') {
            request.respond(200, []);
            return null;
        }

        var findQuery = convertFilter(queryComponents.filters);
        if (findQuery === null) {
            request.respond(500, { error: 'Unsupported filter: ' + queryComponents.filters.queryString });
            return null;
        }
        
        return findQuery;
    }

    function convertFilter(filters) {
        var findQuery = {};

        var startsWith = [ /^startswith\(([^,]+),\'([^\']+)\'\)/, function(p) {
            var field = p[1];
            var value = p[2];
            var result = {};
            result[field] = new RegExp('^' + value);
            return result;
        } ];
        
        var binaryOperator = [ /^\(([^\s]+)\s+([^\s]{2})\s(.+)$/, function(p) {
            var field = p[1];
            var operator = p[2];
            var value = p[3].slice(0, -1); // remove ending ')'
            if (/datetime\'\d{4}-\d{2}-\d{2}T\d{2}\:\d{2}\:\d{2}\.\d{3}Z\'/.test(value)) {
                // Date literal
                value = new Date(Date.parse(value.slice(9, -1)));
            } else if (/^\'.+\'$/.test(value)) {
                // String literal
                value = value.slice(1, -1);
            } else {
                // Number
                value = parseFloat(value);
            }

            var result = {};
            if (operator === 'eq') {
                result[field] = value;
            } else {
                result[field] = {};
                result[field]['$' + operator] = value;
            }
            return result;
        } ];

        var supportedFilters = [startsWith, binaryOperator];

        if (filters) {
            // Easy cases
            if (filters.queryString === 'true') {
                return {};
            }

            var foundMatch = false;
            for (var i = 0; i < supportedFilters.length; i++) {
                var match = filters.queryString.match(supportedFilters[i][0]);
                if (match) {
                    findQuery = supportedFilters[i][1](match);
                    foundMatch = true;
                    break;
                }
            }
            
            if (!foundMatch) {
                return null;
            }
        }

        return findQuery;
    }
}
