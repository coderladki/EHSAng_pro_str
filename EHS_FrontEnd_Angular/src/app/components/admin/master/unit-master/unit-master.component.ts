import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { first, Subscription } from 'rxjs';
import {
    UnitMaster,
    UnitMasterAddorUpdate,
} from 'src/app/interfaces/UnitMaster.class';
import { MasterInterFace } from 'src/app/masterInterface';
import { UserModel } from 'src/app/models/auth/user.model';
import { AuthService } from 'src/app/services/auth/auth.service';
import { MasterService } from 'src/app/services/masters/master.service';
import { UnsubscriberContainerComponent } from 'src/app/utility/unsubscriber-container/unsubscriber-container.component';
import { UtilityService } from 'src/app/utility/utility.service';

@Component({
    selector: 'app-unit-master',
    templateUrl: './unit-master.component.html',
    styleUrls: ['./unit-master.component.scss'],
})
export class UnitMasterComponent implements OnInit, OnDestroy {
    displayBasic: boolean;
    allUnits: UnitMaster[] = [];
    divisionList: any[] = [];
    selectedModules: any;
    selectedMobileFile: any;
    selectedWebFile: any;
    modules: any = [
        {
            id: 1,
            name: 'WorkPermit',
        },
        {
            id: 2,
            name: 'Quiz',
        },
        {
            id: 3,
            name: 'LearningAuthoriz',
        },
        {
            id: 4,
            name: 'Task',
        },
        {
            id: 5,
            name: 'Accident',
        },
        {
            id: 6,
            name: 'CapaScheduling',
        },
    ];
    iUnit: UnitMasterAddorUpdate = new UnitMasterAddorUpdate();
    private subs = new UnsubscriberContainerComponent();
    getDivisions: MasterInterFace[] = [];
    modalTitle: string = '';
    actionMode: string = '';
    constructor(
        private router: Router,
        private masterservice: MasterService,
        private utility: UtilityService,
        private authService: AuthService
    ) {}

    ngOnInit(): void {
        this.getUnitAll();
        this.getDivision();
    }

    showBasicDialog() {
        this.displayBasic = true;
    }
    getUnitAll() {
        this.utility.showSpinner();
        this.subs.add = this.masterservice
            .masterGetMethod('/unit/getall')
            .subscribe((res) => {
                this.utility.hideSpinner();
                if (res.status == 'ok') {
                    this.allUnits = res.result;
                } else {
                    alert(res.message);
                }
            });
    }
    getDivision() {
        this.subs.add = this.masterservice
            .masterGetMethod('/division/getall')
            .subscribe((res) => {
                this.divisionList = res.Result;
            });
    }
    loginPlantAdmin(unit: any) {
        this.subs.add = this.authService
            .login(unit.unitUserName, unit.unitUserPassword, true)
            .pipe(first())
            .subscribe((user: UserModel | undefined) => {
                if (user) {
                    this.router.navigateByUrl('/dashboard/plant-admin');
                    window.location.reload();
                } else {
                    //this.hasError = true;
                }
            });
    }
    changeModalTitle(type: string) {
        if (type == 'add') {
            this.modalTitle = 'Add Unit';
            this.actionMode = 'add';
            this.iUnit = new UnitMasterAddorUpdate();
            this.iUnit.UnitId = '0';
        }
        if (type == 'edit') {
            this.modalTitle = 'Edit Unit';
            this.actionMode = 'edit';
        }
        this.displayBasic = true;
    }
    onSubmitBtn() {
        if (this.actionMode == 'add') {
            this.addUnit();
        } else {
            this.editUnit();
        }
    }
    addUnit() {
        console.log(this.iUnit);
        this.subs.add = this.masterservice
            .masterPostMethod('/unit/create', this.iUnit)
            .subscribe((res) => {
                if (res.status == 'ok') {
                    this.displayBasic = false;
                    this.getUnitAll();
                    this.utility.showSuccessAlert(res.message);
                }
            });
    }
    // getUnitById(unitId: string) {
    //     this.subs.add = this.masterservice
    //         .masterGetMethod(`/unit/${unitId}`)
    //         .subscribe((res) => {
    //             if (res.status == 'ok') {
    //                 console.log(res.result);
    //             }
    //         });
    // }
    onChangeMobileFile(event: any) {
        this.selectedMobileFile = event.target.files[0];
    }
    onChangeWebFile(event: any) {
        this.selectedWebFile = event.target.files[0];
    }
    editUnit() {
        var _modules = [];
        for (let module of this.selectedModules) {
            _modules.push(module.id);
        }
        this.iUnit.Modoules = _modules.join(',');
        this.iUnit.UnitStatus = true;
        let formData = new FormData();
        formData.append('UnitId', this.iUnit.UnitId);
        formData.append('UnitCode', this.iUnit.UnitCode);
        formData.append('UnitDisplayName', this.iUnit.unitDisplayName);
        formData.append('DivisionId', this.iUnit.DivisionId);
        formData.append('UnitUserName', this.iUnit.UnitUserName);
        formData.append('UnitUserPassword', this.iUnit.UnitUserPassword);
        formData.append('UnitEHSHead', this.iUnit.UnitEHSHead);
        formData.append('UnitEHSAdmin', this.iUnit.UnitEHSAdmin);
        formData.append('UnitHead', this.iUnit.UnitHead);
        formData.append('UnitStatus', this.iUnit.UnitStatus.toString());
        formData.append('Modoules', this.iUnit.Modoules);
        formData.append('MobileFile', this.selectedMobileFile);
        formData.append('WebFile', this.selectedWebFile);
        this.subs.add = this.masterservice
            .masterPostFormDataMethod('/unit/update', formData)
            .subscribe({
                next: (res) => {
                    if (res.status == 'ok') {
                        this.utility.showSuccessAlert(res.message);
                        this.displayBasic = false;
                    }
                },
                error: (err) => {
                    console.error(err);
                },
            });
    }
    onEditUnitbtn(row: any) {
        this.iUnit.UnitId = row.unitId;
        this.iUnit.UnitCode = row.unitCode;
        this.iUnit.DivisionId = row.divisionId;
        this.iUnit.unitDisplayName = row.unitDisplayName;
        this.iUnit.UnitName = row.unitUserName;
        this.iUnit.UnitUserName = row.unitUserName;
        this.iUnit.UnitUserPassword = row.unitUserPassword;
        this.iUnit.UnitEHSHead = row.unitEHSHead;
        this.iUnit.UnitEHSAdmin = row.unitEHSAdmin;
        this.iUnit.UnitHead = row.unitHead;
        this.iUnit.Modoules = row.modules;
        this.iUnit.UnitStatus = true;
        var oldmodules = [];
        for (let oldModule of row.modules.split(',')) {
            let splitData = oldModule.split('-');
            oldmodules.push({
                id: Number(splitData[0]),
                name: splitData[1].trim(),
            });
        }
        this.selectedModules = oldmodules;
        this.changeModalTitle('edit');
    }

    onDeleteUnitBtn(row: any) {
        let confirmText =
            'Are you sure want to delete unit: ' + row.unitDisplayName;
        if (confirm(confirmText) == true) {
            this.subs.add = this.masterservice
                .masterPostMethod(`/unit/delete?id=${row.unitId}`, '')
                .subscribe({
                    next: (res) => {
                        if (res.status == 'ok') {
                            this.utility.showSuccessAlert(res.message);
                            this.getUnitAll();
                        }
                    },
                    error: (err) => {
                        console.log(err);
                    },
                });
        }
    }
    ngOnDestroy(): void {
        this.subs.dispose();
    }
}
