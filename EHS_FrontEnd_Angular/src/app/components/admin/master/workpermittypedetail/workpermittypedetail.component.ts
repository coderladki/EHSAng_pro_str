import { AfterViewChecked, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { Note } from 'src/app/interfaces/Note.interface';
import { PPPEType } from 'src/app/interfaces/PPPEType.class';
import { Precaution } from 'src/app/interfaces/Precaution.class';
import { AuthService } from 'src/app/services/auth/auth.service';
import { MasterService } from 'src/app/services/masters/master.service';
import { UnsubscriberContainerComponent } from 'src/app/utility/unsubscriber-container/unsubscriber-container.component';
import { UtilityService } from 'src/app/utility/utility.service';

@Component({
  selector: 'app-workpermittypedetail',
  templateUrl: './workpermittypedetail.component.html',
  styleUrls: ['./workpermittypedetail.component.scss']
})
export class WorkpermittypedetailComponent implements OnInit, AfterViewChecked, OnDestroy {


  TypeName: string = ""
  MasterPPPEList: PPPEType[];
  DefaultPrecauitonsMaster: Precaution[]
  PPPEValue: string = ""
  TemplateName: string = ""
  HazardName: string = ""
  DisplayOrder: string = ""
  OptionType: string = ""
  OptionValue: string = ""
  AskNotes: boolean = false

  PrecautionId: string = ""
  PrecautionList: []

  Notes: Note[] = []
  NewNote: string = ""

  VerificationText: string
  VerificationDisplayOrderText: string
  VerificationWeightageText: string


  PPPEList: any[]

  private subs = new UnsubscriberContainerComponent();
  private unsubscribe: Subscription[] = [];

  constructor(private route: ActivatedRoute, private masterservice: MasterService, private utility: UtilityService,
    private authService: AuthService) {

  }
  ngOnInit(): void {
    this.TypeName = this.route.snapshot.params['TypeName']
    this.getPPPEListMaster()
    this.getPrecautionListMaster()
  }
  ngAfterViewChecked(): void {
  }

  setPPPElist() {
  }
  getPPPEListMaster() {

    this.masterservice
      .masterGetMethod('/PPPETypeMaster/getall')
      .subscribe((res) => {
        console.log(res)
        if (res.Status == "ok") {
          this.MasterPPPEList = res.Result;
        }
      });
  }

  getPrecautionListMaster() {
    this.masterservice
      .masterGetMethod('/DefaultPrecautions/getall')
      .subscribe((res) => {
        console.log(res)
        if (res.Status == "ok") {
          this.DefaultPrecauitonsMaster = res.Result;
        }
      });
  }

  setNotes() {
    this.Notes = [{ Id: 1, NoteDesc: "note1" }, { Id: 1, NoteDesc: "note2" }]
  }

  ngOnDestroy() {
    this.unsubscribe.forEach((sb) => sb.unsubscribe());
  }



}
