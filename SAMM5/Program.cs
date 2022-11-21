var N = 1000;
var lyambda = 12;
var my = 4.5;

var random = new Random();
var timeBeforeApplication = aDistribution(lyambda);
var timeUntilEndOfChannel = new double[3];
var channel = new int[3];
var queue = new int[3];
var status = 0;

var quantityInQueue = 0;
var quantityInSystem = 0;

var timeInQueue = 0.0d;
var timeInSystem = 0.0d;

var time = 0.0d;
while (time <= N)
{
    if (timeBeforeApplication == 0)
    {
        int i = status;
        if (channel[i] == 0)
        {
            channel[i] = 1;
            quantityInSystem++;
            timeUntilEndOfChannel[i] = aDistribution(my);
        }
        else
        {
            if (queue[i] < 6)
            {
                queue[i]++;
                quantityInQueue++;
                quantityInSystem++;
            }
        }
        switch (i)
        {
            case 0:
                status = 1;
                break;
            case 1:
                status = 2;
                break;
            case 2:
                status = 0;
                break;
        }
        timeBeforeApplication = aDistribution(lyambda);
    }

    for (int i = 0; i < 3; i++)
    {
        if (timeUntilEndOfChannel[i] == 0 && channel[i] == 1)
        {
            channel[i] = 0;
            if (queue[i] > 0)
            {
                queue[i]--;
                channel[i] = 1;
                timeUntilEndOfChannel[i] = aDistribution(my);
            }
        }
    }


    var minTimeUntilEndOfChannel = 0.0d;
    if (channel[0] == 0 && channel[1] == 0 && channel[2] == 0)
    {
        minTimeUntilEndOfChannel = -1;
    }
    else
    {
        if (channel[0] == 0 && channel[1] == 0 && channel[2] == 1)
        {
            minTimeUntilEndOfChannel = timeUntilEndOfChannel[2];
        }
        else
        {
            if (channel[0] == 0 && channel[1] == 1 && channel[2] == 0)
            {
                minTimeUntilEndOfChannel = timeUntilEndOfChannel[1];
            }
            else
            {
                if (channel[0] == 1 && channel[1] == 0 && channel[2] == 0)
                {
                    minTimeUntilEndOfChannel = timeUntilEndOfChannel[0];
                }
                else
                {
                    if (channel[0] == 0 && channel[1] == 1 && channel[2] == 1)
                    {
                        if (timeUntilEndOfChannel[1] < timeUntilEndOfChannel[2])
                        {
                            minTimeUntilEndOfChannel = timeUntilEndOfChannel[1];
                        }
                        else
                        {
                            minTimeUntilEndOfChannel = timeUntilEndOfChannel[2];
                        }
                    }
                    else
                    {
                        if (channel[0] == 1 && channel[1] == 0 && channel[2] == 1)
                        {
                            if (timeUntilEndOfChannel[0] < timeUntilEndOfChannel[2])
                            {
                                minTimeUntilEndOfChannel = timeUntilEndOfChannel[0];
                            }
                            else
                            {
                                minTimeUntilEndOfChannel = timeUntilEndOfChannel[2];
                            }
                        }
                        else
                        {
                            if (channel[0] == 1 && channel[1] == 1 && channel[2] == 0)
                            {
                                if (timeUntilEndOfChannel[1] < timeUntilEndOfChannel[0])
                                {
                                    minTimeUntilEndOfChannel = timeUntilEndOfChannel[1];
                                }
                                else
                                {
                                    minTimeUntilEndOfChannel = timeUntilEndOfChannel[0];
                                }
                            }
                            else
                            {
                                if (channel[0] == 1 && channel[1] == 1 && channel[2] == 1)
                                {
                                    if (timeUntilEndOfChannel[0] < timeUntilEndOfChannel[1] && timeUntilEndOfChannel[0] < timeUntilEndOfChannel[2])
                                    {
                                        minTimeUntilEndOfChannel = timeUntilEndOfChannel[0];
                                    }
                                    else
                                    {
                                        if (timeUntilEndOfChannel[1] < timeUntilEndOfChannel[0] && timeUntilEndOfChannel[1] < timeUntilEndOfChannel[2])
                                        {
                                            minTimeUntilEndOfChannel = timeUntilEndOfChannel[1];
                                        }
                                        else
                                        {
                                            minTimeUntilEndOfChannel = timeUntilEndOfChannel[2];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    double temp;
    if (timeBeforeApplication < minTimeUntilEndOfChannel || minTimeUntilEndOfChannel == -1)
    {
        temp = timeBeforeApplication;
        timeBeforeApplication = 0;
        for (int j = 0; j < 3; j++)
        {
            timeUntilEndOfChannel[j] -= temp;
        }
        timeInQueue += temp * (queue[0] + queue[1] + queue[2]);
        timeInSystem += temp * (queue[0] + queue[1] + queue[2] + channel[0] + channel[1] + channel[2]);
    }
    else
    {
        temp = minTimeUntilEndOfChannel;
        timeBeforeApplication -= temp;
        for (int j = 0; j < 3; j++)
        {
            timeUntilEndOfChannel[j] -= temp;
        }
        timeInQueue += temp * (queue[0] + queue[1] + queue[2]);
        timeInSystem += temp * (queue[0] + queue[1] + queue[2] + channel[0] + channel[1] + channel[2]);
    }

    time += temp;
}

var messsage =
$@"Среднее число заявок в очереди = {timeInQueue / N}
Среднее число заявок в системе = {timeInSystem / N} 
Среднее время в очереди = {timeInQueue / quantityInSystem}
Среднее время в системе = {timeInSystem/ quantityInSystem}";
Console.WriteLine(messsage);

double aDistribution(double lambda)
{
    return -Math.Log(random.NextDouble()) / lambda;
}