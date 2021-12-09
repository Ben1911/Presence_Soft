using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using GestionServeurGUI;

namespace ManagerConnection
{
    public class Connexion
    {
        public Connexion()
        {

        }

        private  string _serveur = "localhost";
        private string _database = "";
        private string _user = "";
        private string _password = "";
        private  int _port = 0;

        public string Serveur
        {
            get
            {
                return _serveur;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Please specify a valid Server !!!");
                else
                    _serveur = value;
            }
        }

        public string Database
        {
            get
            {
                return _database;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Please specify a valid Database !!!");
                else
                    _database = value;
            }
        }

        public string User
        {
            get
            {
                return _user;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Please specify a valid Username !!!");
                else
                    _user = value;
            }
        }

        public  string Password
        {
            get
            {
                return _password;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Please specify a valid Password !!!");
                else
                    _password = value;
            }
        }

        public int Port
        {
            get
            {
                return _port;
            }

            set
            {
                if (value <= 0)
                    throw new InvalidOperationException("Please specify a valid Port number !!!");
                else
                    _port = value;
            }
        }
        public static bool TestFile()
        {
            bool test = false;
            if (Directory.Exists(ClsConstante.Table.InitialDirectory) == true)
            { test = true; }
            else
            {
                Directory.CreateDirectory(ClsConstante.Table.InitialDirectory);
                test = true;
            }
            if (File.Exists(ClsConstante.Table.chemin) == true)
            {
                test = true;
            }
            else
            {

                test = false;
            }
            return test;
        }
    }
}
