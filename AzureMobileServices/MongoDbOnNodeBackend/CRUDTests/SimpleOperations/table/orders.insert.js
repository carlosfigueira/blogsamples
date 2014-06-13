function insert(item, user, request) {
    var collectionName = tables.current.getTableName();
    var mongoHelper = require('../shared/mongoHelper');
    mongoHelper.connectAndGetCollection(collectionName, function(err, collection) {
        if (err) {
            console.log('Error creating collection: ', err);
            request.respond(500, { error: err });
            return;
        }

        if (Array.isArray(item)) {
            request.respond(400, { error: 'Bulk inserts not supported' });
            return;
        }

        // Normalize the id to what MongoDB expects
        mongoHelper.mobileServiceIdToMongoId(item);

        collection.insert(item, { w: 1 }, function(err, result) {
            if (err) {
                console.log('Error inserting into the collection: ', err);
                request.respond(500, { error: err });
                return;
            }

            // Unwrap the inserted item
            result = result[0];
            
            // Normalize the id to what the mobile service client expects
            mongoHelper.mongoIdToMobileServiceId(result);

            request.respond(201, result);
        });
    });
}
