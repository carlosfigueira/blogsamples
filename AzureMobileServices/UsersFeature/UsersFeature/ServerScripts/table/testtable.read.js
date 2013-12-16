function read(query, user, request) {
    user.getIdentities({
        success: function (identities) {
            request.respond(200, identities);
        }
    });
}