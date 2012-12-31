using System;
using cuckoosharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Example
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using (CuckooSession session = new CuckooSession("127.0.0.1", 8090))
			{
				using (CuckooManager manager = new CuckooManager(session))
				{
					FileTask task = new FileTask();
					task.Filepath = "/var/www/payload.exe";
					
					int taskID = manager.CreateTask(task);
					
					while((task = (FileTask)manager.GetTaskDetails(taskID)).Status == "pending" || task.Status == "processing")
					{
						Console.WriteLine("Waiting 30 seconds..."+task.Status);
						System.Threading.Thread.Sleep(30000);
					}
					
					if (task.Status == "failure")
					{
						foreach (object error in task.Errors)
							Console.WriteLine(error.ToString());
						
						return;
					}
					
					string report = manager.GetTaskReport(taskID).ToString();
					
					Console.WriteLine(report);
				}
			}
		}
	}
}
