import { Injectable } from '@angular/core';
import Dexie from 'dexie';

@Injectable({
  providedIn: 'root'
})
export class DatabaseService extends Dexie {

  constructor() {
    super('signalRSample');
  }
}
