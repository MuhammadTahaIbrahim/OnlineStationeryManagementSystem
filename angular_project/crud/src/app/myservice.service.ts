import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MyserviceService {
readonly url='http://localhost:3000/user1'
  constructor(private http:HttpClient) { }

  // insert all user

  User_Record(data:any){
    return this.http.post(this.url,data)
  }

  // get all user on table

  GetAllUser(){
    return this.http.get(this.url);
  }

  // delete

  delete_id(id:any){
    return this.http.delete(`${this.url}/${id}`);
  }

  // get user by id

  GetUserbyId(id:number){
    return this.http.get(`${this.url}/${id}`)
  }

  // finally update user

  updateStudentDetails(id:number, data:any){
      return this.http.put(`${this.url}/${id}`,data)
  }
}
