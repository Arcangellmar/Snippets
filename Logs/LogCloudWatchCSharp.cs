using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Newtonsoft.Json;

namespace <YourProject>.Utils
{
    public class Logs
    {
        readonly static IAmazonCloudWatchLogs cloudWatchLogsClient = new AmazonCloudWatchLogsClient();

        public static async void CrearLog(string? LogGroupName, object? logMessage)
        {
            try
            {
                LogGroupName = "/aws/projects/<YourProject>-" + LogGroupName;

                string logStreamName = DateTime.Now.ToString("yyyy/MM/dd");

                var describeLogGroupsRequest = new DescribeLogGroupsRequest
                {
                    LogGroupNamePrefix = LogGroupName
                };

                var describeLogGroupsResponse = await cloudWatchLogsClient.DescribeLogGroupsAsync(describeLogGroupsRequest);


                if (describeLogGroupsResponse.LogGroups.Count == 0)
                {
                    // Si el stream no existe, se crea uno nuevo
                    var createLogRequest = new CreateLogGroupRequest
                    {
                        LogGroupName = LogGroupName,
                    };
                    await cloudWatchLogsClient.CreateLogGroupAsync(createLogRequest);
                }



                // Obtener o crear el stream de registros
                var describeStreamsRequest = new DescribeLogStreamsRequest
                {
                    LogGroupName = LogGroupName,
                    LogStreamNamePrefix = logStreamName
                };

                var describeStreamsResponse = await cloudWatchLogsClient.DescribeLogStreamsAsync(describeStreamsRequest);

                if (describeStreamsResponse.LogStreams.Count == 0)
                {
                    // Si el stream no existe, se crea uno nuevo
                    var createStreamRequest = new CreateLogStreamRequest
                    {
                        LogGroupName = LogGroupName,
                        LogStreamName = logStreamName
                    };
                    await cloudWatchLogsClient.CreateLogStreamAsync(createStreamRequest);
                }


                // Agregar el evento al grupo de registros
                var putLogEventsRequest = new PutLogEventsRequest
                {
                    LogGroupName = LogGroupName,
                    LogStreamName = logStreamName,
                    LogEvents = new List<InputLogEvent>
                    {
                        new InputLogEvent
                        {
                            Timestamp = DateTime.Now,
                            Message = JsonConvert.SerializeObject(logMessage)
                        }
                    }
                };
                await cloudWatchLogsClient.PutLogEventsAsync(putLogEventsRequest);
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
