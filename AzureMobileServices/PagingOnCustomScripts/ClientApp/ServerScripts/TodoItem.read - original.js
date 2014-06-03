function read(query, user, request) {
    var table = tables.current;
    var queryComponents = query.getComponents();
    var take = queryComponents.take;
    var skip = queryComponents.skip;
    var sql = 'SELECT id, text, complete FROM ' + table.getTableName();
    sql = sql + ' ORDER BY text';
    sql = sql + ' OFFSET ' + skip + ' ROWS ';
    sql = sql + ' FETCH NEXT ' + take + ' ROWS ONLY';
    console.log('sql: ' + sql);
    mssql.query(sql, {
        success: function (results) {
            request.respond(200, results);
        }
    });
}