import { Component } from '@angular/core';
import { MyserviceService } from '../myservice.service';
import { ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-updata',
  templateUrl: './updata.component.html',
  styleUrl: './updata.component.css'
})
export class UpdataComponent {

  constructor(private myservice:MyserviceService, private router :ActivatedRoute){}
  
  // create form input variable

  edituser = new FormGroup({
    fullname: new FormControl(''),
    email: new FormControl(''),
    mobile: new FormControl(''),
    age:new FormControl('')
  });


// get user by id 

ngOnInit(): void{

  console.log(this.router.snapshot.params['id']);
  this.myservice.GetUserbyId(this.router.snapshot.params['id']).subscribe((result:any)=>{

    // console.log(result);

    this.edituser = new FormGroup({
      fullname: new FormControl(result['fullname']),
      email: new FormControl(result['email']),
      mobile: new FormControl(result['mobile']),
      age: new FormControl(result['age'])
    });
  });
}

  // update user record

  message:boolean= false

  UpdateUser(){
    // console.log(this.edituser.value);

    this.myservice.updateStudentDetails(this.router.snapshot.params['id'],this.edituser.value).subscribe((res)=>{
      console.log(res);
      this.message=true;
      this.edituser.reset();

    })
  }
    // alert dialog

    removeMessage(){
      this.message=false;
      this.edituser.reset();
    }

}
