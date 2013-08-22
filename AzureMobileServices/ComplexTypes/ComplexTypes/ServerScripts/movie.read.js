function read(query, user, request) {
    request.execute({
        success: function (movies) {
            var reviewsTable = tables.getTable('MovieReview');
            var readReviewsForMovie = function (movieIndex) {
                if (movieIndex >= movies.length) {
                    request.respond();
                } else {
                    reviewsTable.where({ movieId: movies[movieIndex].id }).read({
                        success: function (reviews) {
                            movies[movieIndex].reviews = reviews;
                            readReviewsForMovie(movieIndex + 1);
                        }
                    });
                }
            };

            readReviewsForMovie(0);
        }
    });
}
