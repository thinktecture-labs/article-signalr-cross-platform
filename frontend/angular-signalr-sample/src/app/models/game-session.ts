import { User } from './user';

export interface GameSession {
  sessionId: string;
  userOne: User;
  userTwo: User;
  activeUser: string;
  moves: KeyValue[];
}

export interface KeyValue {
  key: string;
  value: any;
}
