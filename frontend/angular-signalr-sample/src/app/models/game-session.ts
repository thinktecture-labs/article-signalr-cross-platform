export interface GameSession {
  sessionId: string;
  userOne: string;
  userTwo: string;
  activeUser: string;
  moves: KeyValue[];
}

export interface KeyValue {
  key: string;
  value: any;
}
