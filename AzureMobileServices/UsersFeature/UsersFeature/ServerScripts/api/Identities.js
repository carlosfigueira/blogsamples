exports.get = function (request, response) {
    request.user.getIdentities({
        success: function (identities) {
            response.send(statusCodes.OK, identities);
        }
    });
};
