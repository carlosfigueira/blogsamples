exports.get = function (request, response) {
    var user = request.user;
    user.getIdentities({
        success: function (identities) {
            var req = require('request');
            var url = null;
            var oauth = null;
            var userId = user.userId.split(':')[1];
            console.log('Identities: ', identities);
            if (identities.facebook) {
                url = 'https://graph.facebook.com/me?access_token=' +
                    identities.facebook.accessToken;
            } else if (identities.google) {
                url = 'https://www.googleapis.com/oauth2/v3/userinfo' +
                    '?access_token=' + identities.google.accessToken;
            } else if (identities.microsoft) {
                url = 'https://apis.live.net/v5.0/me?access_token=' +
                    identities.microsoft.accessToken;
            } else if (identities.twitter) {
                var consumerKey = process.env.MS_TwitterConsumerKey;
                var consumerSecret = process.env.MS_TwitterConsumerSecret;
                oauth = {
                    consumer_key: consumerKey,
                    consumer_secret: consumerSecret,
                    token: identities.twitter.accessToken,
                    token_secret: identities.twitter.accessTokenSecret
                };
                url = 'https://api.twitter.com/1.1/users/show.json?' +
                    'user_id=' + userId + '&include_entities=false';
            } else {
                response.send(500, { error: 'No known identities' });
                return;
            }

            if (url) {
                var reqParams = { uri: url, headers: { Accept: 'application/json' } };
                if (oauth) {
                    reqParams.oauth = oauth;
                }
                req.get(reqParams, function (err, resp, body) {
                    if (err) {
                        console.error('Error calling provider: ', err);
                        response.send(500, { error: 'Error calling provider' });
                        return;
                    }

                    if (resp.statusCode !== 200) {
                        console.error('Provider call did not return success: ', resp.statusCode);
                        response.send(500, { error: 'Provider call did not return success: ' + resp.statusCode });
                        return;
                    }

                    try {
                        var userData = JSON.parse(body);
                        response.send(200, userData);
                    } catch (ex) {
                        console.error('Error parsing response: ', ex);
                        response.send(500, { error: ex });
                    }
                });
            } else {
                response.send(500, { error: 'Not implemented yet', env: process.env });
            }
        }
    });
};
