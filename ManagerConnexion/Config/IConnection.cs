using ManagerConnection;
using System;
using System.Data;

namespace ManagerConnection
{
    internal interface IConnection
    {
        IDbConnection Initialise(ConnexionType connectionType);
    }
}
