using System.Collections.Generic;
using UnityEngine;

public class CarTask : MonoBehaviour
{
    public List<TaskType> tasks = new List<TaskType>();
    int currentIndex = 0;

    public void SetupTasks(TaskType[] taskList)
    {
        tasks.Clear();
        tasks.AddRange(taskList);
        currentIndex = 0;
        
    }

    public TaskType GetCurrentTask()
    {
        return tasks[currentIndex];
    }

	public bool isFinished() 
	{
		return currentIndex >= tasks.Count;
	}

    public bool CompleteCurrentTask()
    {
        currentIndex++;
        return currentIndex >= tasks.Count;
    }
}