using SuperSocket.Client;

var easyClient = new EasyClient<RpcPackageBase, RpcPackageBase>(new RpcPipeLineFilter(), new RpcPackageEncode()).AsClient();