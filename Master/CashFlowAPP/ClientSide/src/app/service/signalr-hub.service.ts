import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';
import { FromClientChat } from 'src/app/models/FromClientChat';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalrHubService {

  constructor() {
   }
  IsConnected:boolean=false;
  Connection: HubConnection | any;
  ConnectionID: string | undefined;
  ConnectionUser:string | undefined;
  Connect(UserToken: string) {
    this.Connection =
      new HubConnectionBuilder()
        .withUrl(`${environment.HubRoot}?${UserToken}`)
        .withAutomaticReconnect()
        .build();

    // Starts the connection.
    if(this.ConnectionID==undefined){
      this.Connection.on("UpdSelfID", (ConnectId: any, SelfObj: any) => {
        this.ConnectionID = ConnectId;
        this.ConnectionUser = SelfObj.name;
      });
     this.Connection.start().then((res:any) => {
      this.IsConnected=true;
      this.Invoke("SetTimer", 5);
      this.OnObservable("Okay").subscribe((Res:any)=>{
        console.log(Res,"timer");
      });
    })
    .catch(function (err:any) {
      //failed to connect
      return console.error(err.toString());
    });;
    }
  }



  OnObservable(method: string): Observable<any> {


    // Establishes a Hub Connection with specified url.

    const subject: Subject<any> = new Subject<any>();

    // On reciving an event when the hub method with the specified method name is invoked
    this.Connection.on(method, (...args: any[]) => {
      // Multicast the event.
      subject.next(args);
    });

    // When the connection is closed.
    this.Connection.onclose((err?: Error) => {
      if (err) {
        // An error occurs
        subject.error(err);
      } else {
        // No more events to be sent.
        subject.complete();
      }
    });



    // To be subscribed to by multiple components
    return subject;
  }
  //invoke()
  Invoke(Method:string,Arg:any){
    if(this.IsConnected){
          this.Connection.invoke(Method,Arg).catch(function (err: any) {
      alert('傳送錯誤: ' + err.toString());
    });
    }
  }
}
