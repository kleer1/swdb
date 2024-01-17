import asyncio
import websockets
import argparse
import time
import json


async def connect_to_dotnet_server(port):
    uri = f"ws://127.0.0.1:{port}"

    async with websockets.connect(uri, timeout=5) as websocket:
        try:
            while True:
                # Receive response from the .NET server
                response_from_dotnet = await websocket.recv()
                print(f"Received response from .NET server: {response_from_dotnet}")
                obj = json.loads(response_from_dotnet)
                if obj["Done"] == True:
                    break;
                obj = obj["Observation"]
                play_card = obj.get("PlayCard", [])
                attack_base = obj.get("AttackBase", [])
                select_attacker = obj.get("SelectAttacker", [])
                confirm_attacker = obj.get("ConfirmAttackers", [])
                purchase_card = obj.get("PurchaseCard", [])
                decline = obj.get("DeclineAction", [])
                pass_turn = obj.get("PassTurn", [])
                choose_base = obj.get("ChooseNextBase", [])
                if len(play_card) > 0:
                    msg = json.dumps(play_card[0])
                    await websocket.send(msg)
                    print(f"Sent message to .NET server: {msg}")
                elif len(attack_base) > 0:
                    msg = json.dumps(attack_base[0])
                    await websocket.send(msg)
                    print(f"Sent message to .NET server: {msg}")
                elif len(select_attacker) > 0:
                    msg = json.dumps(select_attacker[0])
                    await websocket.send(msg)
                    print(f"Sent message to .NET server: {msg}")
                elif len(confirm_attacker) > 0:
                    msg = json.dumps(confirm_attacker[0])
                    await websocket.send(msg)
                    print(f"Sent message to .NET server: {msg}")
                elif len(purchase_card) > 0:
                    msg = json.dumps(purchase_card[0])
                    await websocket.send(msg)
                    print(f"Sent message to .NET server: {msg}")
                elif len(decline) > 0:
                    msg = json.dumps(decline[0])
                    await websocket.send(msg)
                    print(f"Sent message to .NET server: {msg}")
                elif len(pass_turn) > 0:
                    msg = json.dumps(pass_turn[0])
                    await websocket.send(msg)
                    print(f"Sent message to .NET server: {msg}")
                elif len(choose_base) > 0:
                    msg = json.dumps(choose_base[0])
                    await websocket.send(msg)
                    print(f"Sent message to .NET server: {msg}")
                else:
                    msg = "Could not decide what action to take"
                    print(f"Could not pick from: {obj}")
                    await websocket.send(msg)
                    print(f"Sent message to .NET server: {msg}")

                time.sleep(10 / 1000)
        except websockets.exceptions.ConnectionClosedOK:
            print("Connection closed by the .NET server.")


def main():
    parser = argparse.ArgumentParser(description="Python client for .NET WebSocket server")
    parser.add_argument("--port", type=int, default=8765, help="Port number of the .NET WebSocket server")
    args = parser.parse_args()

    print(f"Starting web socket connection with .NET server on {args.port}")
    time.sleep(2)
    asyncio.get_event_loop().run_until_complete(connect_to_dotnet_server(args.port))


if __name__ == "__main__":
    main()
