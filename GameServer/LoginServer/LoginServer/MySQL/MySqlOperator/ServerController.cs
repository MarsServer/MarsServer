using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace LoginServer
{
    public class ServerController
    {
        public readonly static ServerController instance = new ServerController();

        private Dictionary<string, Server[]> _servers;

        public Dictionary<string, Server[]> servers
        {
            get {
                if (_servers == null)
                {
                    _servers = GetServerList();
                }
                return  _servers;
            }
        }

        public Dictionary<string, Server[]> GetServerList()
        {
            Dictionary<string, Server[]> _servers = new Dictionary<string, Server[]>();
            List<Region> regions = getAllRegions ();

            foreach (Region r in regions)
            {
                StringBuilder sb_sql = new StringBuilder();
                DataTable dt = null;
                sb_sql.AppendFormat(SQLConstants.MySQL_SERVER_LIST, r.id);

                dt = DBUtility.RunSQLReturnDataTable(sb_sql.ToString());
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                dt = DBUtility.RunSQLReturnDataTable(sb_sql.ToString());
                if (dt.Rows.Count == 0)
                {
                    return _servers;
                }
                int length = dt.Rows.Count;
                for (int i = 0; i < length; i++)
                {
                    Server server = new Server();
                    server.serverId = int.Parse(dt.Rows[i][0].ToString());
                    server.serverName = dt.Rows[i][1].ToString();
                    server.limit = int.Parse(dt.Rows[i][2].ToString());
                    server.ip = dt.Rows[i][3].ToString();
                    server.belong = dt.Rows[i][4].ToString();
                    server.isSwitch = (int.Parse(dt.Rows[i][5].ToString()) == 1);
                    if (_servers.ContainsKey (r.regionName) == false)
                    {

                        _servers.Add(r.regionName, null);
                        _servers[r.regionName] = new Server[length];
                    }
                    _servers[r.regionName][i] = server;
                }
            }
            Debug.Log("*********Server Loading Done!!!!");
            this._servers = _servers;
            return _servers;
        }

        List<Region> getAllRegions()
        {
            List<Region> regions = new List<Region>();

            StringBuilder sb_sql = new StringBuilder();
            DataTable dt = null;
            sb_sql.Append(SQLConstants.MySQL_SERVER_REGION_LIST);

            dt = DBUtility.RunSQLReturnDataTable(sb_sql.ToString());
            if (dt.Rows.Count == 0)
            {
                return regions;
            }

            //set region
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Region region = new Region();
                region.id = dt.Rows[i][0].ToString();
                region.regionName = dt.Rows[i][1].ToString();
                regions.Add(region);
            }
            return regions;
        }
    }
}
