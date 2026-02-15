import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from "@microsoft/signalr";

export class SignalRService {
  private connection: HubConnection;

  constructor(
    hubUrl: string,
  ) {
    this.connection = new HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .build();
  }

  async start(): Promise<void> {
    if (this.connection.state === HubConnectionState.Disconnected) {
      await this.connection.start();
    }
  }

  async stop(): Promise<void> {
    if (this.connection.state !== HubConnectionState.Disconnected) {
      await this.connection.stop();
    }
  }

  on<T>(event: string, handler: (data: T) => void) {
    this.connection.on(event, handler);
  }

  off(event: string) {
    this.connection.off(event);
  }

  async invoke<T = void>(method: string, ...args: any[]): Promise<T> {
    return await this.connection.invoke(method, ...args);
  }

  get state() {
    return this.connection.state;
  }
}
