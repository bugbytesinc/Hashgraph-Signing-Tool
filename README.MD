# Hashgraph Signing Tool
The Hashgraph Signing Tool is a reference implementation example for the [.NET Core Client Library for Hedera Hashgraph](https://github.com/bugbytesinc/Hashgraph).

A compiled implementation is presently undergoing certification for inclusion in the [Windows Store](https://www.microsoft.com/en-us/store/apps/windows).

The Hashgraph Signing Tool is a utility that signs Hedera Network Transactions created by other applications (natively encoded in the Hedera [protobuf format](https://github.com/hashgraph/hedera-protobuf), also known as the “[BodyBytes](https://github.com/hashgraph/hedera-protobuf/blob/master/src/main/proto/Transaction.proto)” for those familiar with the low-level [gRPC](https://grpc.io/) protobuf).  The primary purpose for the existence of this utility is to provide one example of how to separate private keys for signing Hedera transactions from the applications creating and transmitting the transactions.  Private keys are given to this utility, not the programs communicating with the hedera network.  This app is not a wallet, nor is it a key ring.  One must trust the integrity of the windows clipboard to paste private keys from other sources, but once placed inside the app, private keys are only held in memory.  They are not displayed and used only to sign transactions, producing protobuf encoded representations of the signatures for consumption by the originating application.

## Security
- If you do not trust the compilation and deployment of this app in the windows store, _do not use that version_.  You are welcome download this source code and examine yourself, compile or modify it and run it on your own system.
- Additionally, by using this application, you are trusting the integrity of your windows clipboard.  You typically paste the value of the private key from the clipboard, if your computer’s clipboard is compromised by a keylogger, you may lose control of your keys.  As a reminder, this is a signing utility, not a wallet or keyring.

## Compiling and Running

This project relies protobuf support found in .net core 3, previous versions of the .net core framework will not work. (At the time of this writing we are in [3.0.101](https://dotnet.microsoft.com/download/dotnet-core/3.0)).

After installing .net core 3, clone the project:
```
git clone https://github.com/bugbytesinc/Hashgraph-Signing-Tool.git 
```
Then from a command line within the project, the `dotnet` command can compile and run the program in one step:
```
dotnet run .\Hashgraph.SigningTool.sln --project Hashgraph.SigningTool
```
Additionally, the project can be loaded into [Visual Studio](https://visualstudio.microsoft.com/).