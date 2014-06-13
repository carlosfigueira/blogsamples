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
    // TODO: look at query parameters. For now, return all items.
    collection.find().toArray(function(err, items) {
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
}
