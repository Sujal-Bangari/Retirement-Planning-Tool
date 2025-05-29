import { CommonModule} from '@angular/common';
import { Component, Input, Output, EventEmitter, OnInit  } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { GoalserviceService } from '../../services/goalservice.service';

@Component({
  selector: 'app-contribute',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './contribute.component.html',
  styleUrl: './contribute.component.css',
  standalone: true
})
export class ContributeComponent implements OnInit {
  @Input() gId !: number;
  @Input() currentSavings!:number;
  @Input() targetSavings!:number;
  @Input() monthlyContribution!: number;
  @Output() investment= new EventEmitter<any>();
  year:number= new Date().getFullYear();
  monthNumber: number | undefined;
  investmentAmount: number =0;
  months = [ // ✅ Month dropdown values
    { name: 'January', value: 1 },
    { name: 'February', value: 2 },
    { name: 'March', value: 3 },
    { name: 'April', value: 4 },
    { name: 'May', value: 5 },
    { name: 'June', value: 6 },
    { name: 'July', value: 7 },
    { name: 'August', value: 8 },
    { name: 'September', value: 9 },
    { name: 'October', value: 10 },
    { name: 'November', value: 11 },
    { name: 'December', value: 12 }
  ];
  

  constructor(private goalService: GoalserviceService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    console.log("Initializing ContributeComponent..."); // ✅ Debugging step
  
    if (!this.gId || this.gId <= 0 || isNaN(this.gId)) {
      console.warn("Warning: gId is invalid on initialization. Waiting for @Input() update...");
    } else {
      console.log("Final gId in ContributeComponent after initialization:", this.gId); // ✅ Debugging step
    }
    
    console.log("hey",this.currentSavings);
    console.log("hey", this.targetSavings);
    console.log("hey", this.monthlyContribution);
    this.investmentAmount=this.monthlyContribution;
  }
  
  get maxInvestment(): number {
    //return Math.max(this.targetSavings - this.currentSavings, 0); // ✅ Ensures investment does not exceed remaining savings
    return this.targetSavings && this.currentSavings !== undefined
    ? Math.max(this.targetSavings - this.currentSavings, 0)
    : 0;
  }

  submitInvestment() {
    if (!this.gId || !this.monthNumber || !this.investmentAmount) {
      console.error("Error: Missing gId, cannot store investment.");
      return;
    }
    
  const contributionData = {
    gId: this.gId, // ✅ Send `gId` in the request
    year: new Date().getFullYear(),                          //this.year ? Number(this.year) : new Date().getFullYear(),  ✅ Avoid undefined year values
    monthNumber: Number(this.monthNumber),                                                   //Math.min(Math.max(this.monthNumber ?? 0, 1), 12) Ensure monthNumber is between 1-12
    monthlyInvestment: this.investmentAmount ?? 0 // Prevent sending `undefined`
  };
  console.log("Submitting Investment:", contributionData);

    this.goalService.contribute(contributionData).subscribe(
      (response: any) => {
        alert("Investment Submitted Successfully!");
        console.log("Investment Response:", response);
        this.investment.emit();
        
        // setTimeout(() => {
        //   this.investment.emit(response); // Triggers progress update
        // }, 1);
      },
      (error: any) => {
        alert("Check the values");
        console.error("Error while submitting investment:", error);
        console.log("Full API Error Response:", error.error); // Log the backend response
      }
    );
  }
}
