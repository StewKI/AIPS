import {SignalRService} from "@/services/signalr.ts";


const client = new SignalRService(
  `http://localhost:5039/testhub`,
);

export const testHubService = {
  async connect() {
    await client.start();
  },
  /*
  async joinBoard(boardId: string) {
    await client.invoke("JoinBoard", boardId);
  },
*/

  onTest(callback: (text: string) => void) {
    client.on<string>("ReceiveText", callback);
  }
};
