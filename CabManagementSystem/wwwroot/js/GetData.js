function setProp(prop, id, conectionString) {
    var connection = new ActiveXObject("adodb.Connection");

    connection.Open(conectionString);
    var rs = new ActiveXObject("adodb.Recordset");

    var suggestion = rs.Open(`SELECT ${prop} FROM Taxi WHERE ID = ${id}`, connection);
    rs.Close();
    connection.Close();
    return suggestion;
}