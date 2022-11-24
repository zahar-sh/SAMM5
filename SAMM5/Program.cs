using System.Globalization;

Console.Write("Enter N: ");
var count = (int)ParseFloat(Console.ReadLine());
Console.Write("Enter lambda: ");
var lambda = ParseFloat(Console.ReadLine());
Console.Write("Enter my: ");
var my = ParseFloat(Console.ReadLine());

var random = new Random();
var timeBeforeApplication = NextDouble(lambda);

var timeUntilEndOfChannel = new double[3];
var channel = new int[3];
var queue = new int[3];
var status = 0;

var quantityInQueue = 0;
var quantityInSystem = 0;

var timeInQueue = 0.0d;
var timeInSystem = 0.0d;

var time = 0.0d;
while (time <= count)
{
    if (timeBeforeApplication < 1.0E-30)
    {
        if (channel[status] == 0)
        {
            channel[status] = 1;
            quantityInSystem++;
            timeUntilEndOfChannel[status] = NextDouble(my);
        }
        else
        {
            if (queue[status] < 6)
            {
                queue[status]++;
                quantityInQueue++;
                quantityInSystem++;
            }
        }
        status++;
        status %= 3;
        timeBeforeApplication = NextDouble(lambda);
    }

    for (int i = 0; i < 3; i++)
    {
        if (channel[i] == 1 && timeUntilEndOfChannel[i] < 1.0E-30)
        {
            channel[i] = 0;
            if (queue[i] > 0)
            {
                queue[i]--;
                channel[i] = 1;
                timeUntilEndOfChannel[i] = NextDouble(my);
            }
        }
    }

    var minTimeUntilEndOfChannel = double.MaxValue;
    for (int i = 0; i < 3; i++)
    {
        if (channel[i] == 1)
        {
            minTimeUntilEndOfChannel = Math.Min(minTimeUntilEndOfChannel, timeUntilEndOfChannel[i]);
        }
    }

    var minTime = Math.Min(timeBeforeApplication, minTimeUntilEndOfChannel);
    timeBeforeApplication -= minTime;
    for (int j = 0; j < 3; j++)
    {
        timeUntilEndOfChannel[j] -= minTime;
    }
    timeInQueue += minTime * (queue[0] + queue[1] + queue[2]);
    timeInSystem += minTime * (queue[0] + queue[1] + queue[2] + channel[0] + channel[1] + channel[2]);
    time += minTime;
}

var messsage =
$@"Lo = {timeInQueue / count}
Lc = {timeInSystem / count} 
Wo = {timeInQueue / quantityInSystem}
Wc = {timeInSystem/ quantityInSystem}";

Console.WriteLine(messsage);

double NextDouble(double lambda)
{
    return -Math.Log(random.NextDouble()) / lambda;
}

double ParseFloat(string s)
{
    return double.Parse(s, CultureInfo.InvariantCulture);
}