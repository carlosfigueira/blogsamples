function del(itemId, user, request) {
    var collectionName = tables.current.getTableName();
    var mongoHelper = require('../shared/mongoHelper');
    mongoHelper.connectAndGetCollection(collectionName, function(err, collection) {
        if (err) {
            console.log('Error creating collection: ', err);
            request.respond(500, { error: err });
            return;
        }

        collection.remove(mongoHelper.queryForId(itemId), { w: 1 }, function(err, result) {
            if (err) {
                console.log('Error deleting item in the collection: ', err);
                request.respond(500, { error: err });
                return;
            }

            if (result) {
                request.respond(204);
            } else {
                request.respond(404);
            }
        });
    });
}
