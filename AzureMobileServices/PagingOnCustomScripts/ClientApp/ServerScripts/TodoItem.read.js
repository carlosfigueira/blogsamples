function read(query, user, request) {
    var table = tables.current;
    var queryComponents = query.getComponents();
    var take = queryComponents.take;
    var skip = queryComponents.skip;
    var sql = 'SELECT id, text, complete FROM ' + table.getTableName();
    sql = sql + ' ORDER BY text';
    sql = sql + ' OFFSET ' + skip + ' ROWS ';
    sql = sql + ' FETCH NEXT ' + take + ' ROWS ONLY';

    // Adding the total count
    sql = sql + '; SELECT COUNT(*) as [count] FROM ' + table.getTableName();

    console.log('sql: ' + sql);

    // Notice that there are two statements in the sql; that means that the
    // callback to mssql.query will be called twice. Let's use some captured
    // variables to identify when all responses have arrived so that we avoid
    // calling 'request.respond' more than once.
    var resultWithTotalCount = {};
    var mssqlCallbackCount = 0;
    mssql.query(sql, {
        success: function (results) {
            console.log('result: ', results);
            if (++mssqlCallbackCount == 1) {
                // Result of first select
                resultWithTotalCount.results = results;
            } else {
                // Result of the 'SELECT COUNT(*)'
                resultWithTotalCount.count = results[0].count;
                request.respond(200, resultWithTotalCount);
            }
        }
    });
}