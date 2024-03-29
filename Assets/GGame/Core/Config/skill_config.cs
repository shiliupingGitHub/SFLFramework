using System.Collections.Generic;
namespace GGame.Core
{
#if Client_Logic
[XLua.LuaCallCSharp]
#endif
	public class skill_config 
	{
		public int id; //ID
		public int attack; //攻击力
		private static Dictionary<int,skill_config> _dic;
		public static Dictionary<int,skill_config> Dic
		{
			get
			{
				if(_dic == null)
				{
					Load();
				}
				return _dic;
			}
		}
		static void Load()
		{
			var text = GResourceServer.Instance.Load<string>("skill_config") as string;
			var listData = LitJson.JsonMapper.ToObject<List<skill_config>>(text);
			_dic = new Dictionary<int, skill_config>();
			foreach (var data in listData)
			{
				_dic[data.id] = data;
			}
		}
	}
}
