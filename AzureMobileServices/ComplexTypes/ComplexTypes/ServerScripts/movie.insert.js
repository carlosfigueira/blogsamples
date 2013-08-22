function insert(item, user, request) {
    var reviews = item.reviews;
    if (reviews) {
        delete item.reviews; // will add in the related table later
    }

    request.execute({
        success: function () {
            var movieId = item.id;
            var reviewsTable = tables.getTable('MovieReview');
            if (reviews) {
                item.reviews = [];
                var insertNextReview = function (index) {
                    if (index >= reviews.length) {
                        // done inserting reviews, respond to client
                        request.respond();
                    } else {
                        var review = reviews[index];
                        review.movieId = movieId;
                        reviewsTable.insert(review, {
                            success: function () {
                                item.reviews.push(review);
                                insertNextReview(index + 1);
                            }
                        });
                    }
                };

                insertNextReview(0);
            } else {
                // no need to do anythin else
                request.respond();
            }
        }
    });
}
