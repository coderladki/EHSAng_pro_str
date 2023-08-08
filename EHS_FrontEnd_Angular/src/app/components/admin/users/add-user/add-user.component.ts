import { ChangeDetectorRef, Component, Injector, OnInit } from '@angular/core';
import { AuthHTTPService } from '../../../../api/services/auth-http.service';
import { UserModel } from '../../../../models/auth/user.model';
import { FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MasterService } from '../../../../services/masters/master.service';
import { BaseComponent } from 'src/app/base/base.component';
import { UtilityService } from 'src/app/utility/utility.service';
interface modal {
    firstname: string;
    lastname: string;
    fullname: string;
    username: string;
    email: string;
}
@Component({
    selector: 'app-add-user',
    templateUrl: './add-user.component.html',
    styleUrls: ['./add-user.component.scss'],
})
export class AddUserComponent extends BaseComponent implements OnInit {
    userModel: UserModel;
    modal: modal;
    public UserForm: FormGroup;
    public RollList: any[];
    spinner: boolean = false;
    constructor(
        injector: Injector,
        private authHttpService: AuthHTTPService,
        private masterService: MasterService,
        private cd: ChangeDetectorRef,
        private fb: FormBuilder,
        private router: Router,
        //private snackBar: SnackBarComponent,
        private utility: UtilityService
    ) {
        super(injector);
    }
    ngOnInit(): void {
        this.initForm();
        this.getRoll();
    }
    CreateNewUser() {
        this.authHttpService.createUser(this.userModel);
    }
    onSubmit() {
        let roles: any[] = [];
        roles.push(this.UserForm.controls['Roll'].value);
        this.UserForm.markAllAsTouched();
        if (this.UserForm.valid) {
            this.utility.showSpinner();
            var requestOject = {
                FirstName: this.UserForm.controls['firstName'].value,
                LastName: this.UserForm.controls['lastName'].value,
                UserName: this.UserForm.controls['userName'].value,
                Password: this.UserForm.controls['Password'].value,
                PhoneNumber: this.UserForm.controls['PhoneNumber'].value,
                Email: this.UserForm.controls['email'].value,
                Status: parseInt(this.UserForm.controls['Status'].value),
                UnitName: this.UserForm.controls['UnitName'].value,
                Roles: roles,
            };
            //console.log(requestOject);
       
            this.masterService
                .masterPostMethod('/users/create', requestOject)
                .subscribe(
                   {
                    next:(res)=>{
                        this.utility.hideSpinner();
                        alert('User added successfully.');
                        this.router.navigate(['/users/user-list']);
                       
                    },
                    error:(errors)=>{
                        let errorMsg = '';
                        if (
                            errors.errors != undefined &&
                            errors.errors != null &&
                            errors.errors.length > 0
                        ) {
                            errors.errors.forEach((e: string) => {
                                errorMsg += e + ' ,';
                            });
                            if (
                                errorMsg.indexOf(
                                    ',',
                                    errorMsg.length - ','.length
                                ) !== -1
                            ) {
                                errorMsg = errorMsg.substring(
                                    0,
                                    errorMsg.length - 1
                                );
                            }

                            alert(errorMsg);
                            this.utility.hideSpinner();
                        } else {
                            alert(errors.description);
                            this.utility.hideSpinner();
                        }
                    }
                   }
                );
        }
    }

    initForm() {
        this.UserForm = this.fb.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            userName: ['', Validators.required],
            Password: ['', Validators.required],
            PhoneNumber: ['', Validators.required],
            email: ['', Validators.required],
            Status: ['', Validators.required],
            Roll: ['', Validators.required],
            UnitName: ['', Validators.required],
        });
    }

    public getRoll() {
        this.utility.showSpinner();
        //let apiUrl = `/roll/getAllRoll`;
        let apiUrl = `/roles/list`;
        this.masterService.masterGetMethod(apiUrl).subscribe(
            (res) => {
                this.utility.hideSpinner();
                this.RollList = res;
                console.log();
                this.cd.detectChanges();
            },
            (error) => {
                this.utility.hideSpinner();
                console.log(error);
            }
        );
    }
}
