using Microsoft.AspNetCore.Mvc;
using MQTT.Client;
using SuperSocket;
using System.ComponentModel.DataAnnotations;

namespace Server.Controllers;

[ApiController]
[Route("order/")]
[Produces("application/json")]
public sealed class OrderController : ControllerBase
{
    private readonly RpcServer _server;
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger, RpcServer server)
    {
        _logger = logger;
        _server = server;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("add/{sessionId}")]
    public async ValueTask<ApiResponse> OrderAddAsync(string sessionId, CancellationToken cancellationToken)
    {
        RpcResponse<OrderAddReply> rpcResponse;
        var response = new ApiResponse { Successful = true };

        try
        {
            rpcResponse = await _server.GetResponseAsync<OrderAddRequest, OrderAddReply>(sessionId, new OrderAddRequest(), cancellationToken);
        }
        catch (Exception ex)
        {
            response.ErrorCode = 500;
            response.Successful = false;
            response.ErrorMessage = ex.Message;
        }

        return response;
    }
}
