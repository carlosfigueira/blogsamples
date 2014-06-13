var MongoClient = require('mongodb').MongoClient;
var ObjectID = require('mongodb').ObjectID;

exports.connectAndGetCollection = function(collectionName, callback) {
    /// <summary>
    /// Connects and obtain a collection from a mongo db. The connection string used to
    ///  connect to the DB must be on the application setting with key "MongoConnString".
    /// </summary>
    /// <param name="collectionName" type="String">The name of the collection to retrieve.</param>
    /// <param name="callback" type="function(error, collection)">The function to be called when the
    ///   collection is retrieved. If there's an error, it will be passed in the first parameter
    ///   of the callback instead.</param>
    var connString = process.env["MongoConnectionString"];
    MongoClient.connect(connString, function(err, db) {
        if (err) {
            callback(err, null);
        } else {
            db.createCollection(collectionName, function(err, collection) {
                if (err) {
                    callback(err, null);
                } else {
                    callback(null, collection);
                }
            });
        }
    });
}

exports.queryForId = function(id) {
    /// <summary>
    /// Returns a query object which can be used to find an object with the given id.
    ///  the query will be a simple query by "_id" based on the value of the id if
    ///  the id is not a valid ObjectID, or a query by the id value or the ObjectID
    ///  value otherwise.
    /// </summary>
    /// <param name="id" type="String">The id to create the query for.</param>
    /// <returns>An object which can be used as the query parameter in MongoDB's
    ///  collection methods "findOne" or "update".</returns>
    if (ObjectID.isValid(id)) {
        return { _id: { $in: [ id, new ObjectID(id) ] } };
    } else {
        return { _id: id };
    }
}

exports.mobileServiceIdToMongoId = function(item, remove) {
    /// <summary>
    /// The REST API for mobile services expects the identifier of the object in the field
    ///  called "id"; for mongodb it's called "_id". This function converts from the mobile
    ///  service to the mongo version.
    /// </summary>
    /// <param name="item" type="Object">The item whose id will be normalized.</param>
    /// <param name="remove" type="Boolean" optional="true">If <code>true</code>, the id is removed
    ///  from the object. If <code>false</code> or not specified, the id is not removed from the
    ///  object.</param>
    /// <returns>The id of the object.</returns>
    var itemId = item.id;
    delete item.id;
    if (itemId && !remove) {
        item._id = itemId;
    }
    
    return itemId;
}

exports.mongoIdToMobileServiceId = function(item, remove) {
    /// <summary>
    /// The REST API for mobile services expects the identifier of the object in the field
    ///  called "id"; for mongodb it's called "_id". This function converts from the mongo
    ///  version to the mobile service version.
    /// </summary>
    /// <param name="item" type="Object">The item whose id will be normalized.</param>
    /// <param name="remove" type="Boolean" optional="true">If <code>true</code>, the id is removed
    ///  from the object. If <code>false</code> or not specified, the id is not removed from the
    ///  object.</param>
    /// <returns>The id of the object.</returns>
    var itemId = item._id;
    delete item._id;
    if (itemId && !remove) {
        item.id = itemId;
    }

    return itemId;
}
