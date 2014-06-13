function update(item, user, request) {
    var collectionName = tables.current.getTableName();
    var mongoHelper = require('../shared/mongoHelper');
    mongoHelper.connectAndGetCollection(collectionName, function(err, collection) {
        if (err) {
            console.log('Error creating collection: ', err);
            request.respond(500, { error: err });
            return;
        }

        // Normalize the id to what MongoDB expects and remove it from the object.
        var itemId = mongoHelper.mobileServiceIdToMongoId(item, true);

        var params = {
            query: mongoHelper.queryForId(itemId),
            sort: [],
            update: { $set: item },
            options: { new: true }
        };

        collection.findAndModify(params.query, params.sort, params.update, params.options, function(err, result) {
            if (err) {
                console.log('Error updating in the collection: ', err);
                request.respond(500, { error: err });
                return;
            }

            if (result) {
                request.respond(200, result);
            } else {
                request.respond(404);
            }
        });
    });
}
