import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
//參考
// 觀察者模式 https://limeii.github.io/2019/07/rxjs-subject/
@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor() { }

  private _Data: any[] = [];
  private _BehaviorSubject = new BehaviorSubject(this._Data);
  SharedData = this._BehaviorSubject.asObservable();

  SetShareData<T>(Data: T[]) {
    this._BehaviorSubject.next(Data);
    console.log('SharedData', this.SharedData);
  }
}
