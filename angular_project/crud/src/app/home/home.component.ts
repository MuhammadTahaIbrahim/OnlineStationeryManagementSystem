import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MyserviceService } from '../myservice.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  // get userr data value
  
  userform = new FormGroup({
    fullname: new FormControl(''),
    email: new FormControl(''),
    mobile: new FormControl(''),
    age:new FormControl('')
  });

  constructor(private service:MyserviceService){}

    // save record

    message:boolean=false;

    submitform(){
      console.log(this.userform.value);
      this.service.User_Record(this.userform.value).subscribe((data)=>{
        // alert('record inserted')
        this.message=true;
      });
    }
    alertclose(){
      this.message=false;
      this.userform.reset();
    }

    // show all user

    fetchdata:any=[];

    ngOnInit(): void {
      this.service.GetAllUser().subscribe((abc)=>{
        // console.log(abc);
        this.fetchdata= abc
      })
    }

    // delete

    delete_user(id:any){
      this.service.delete_id(id).subscribe((res)=>{
        this.ngOnInit();
      })
    }
}
