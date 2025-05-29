import { Component, Output, EventEmitter, Input, OnInit } from '@angular/core';
import { GoalserviceService } from '../../services/goalservice.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-goal',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './add-goal.component.html',
  styleUrl: './add-goal.component.css',
  standalone: true
})
export class AddGoalComponent implements OnInit {
  @Input() pId!: number;
  @Input() age!: number;
  @Output() goalAdded = new EventEmitter<any>(); 
  
  userData: any;

  gId: number | undefined;
  currentAge: number = 0;
  retirementAge: number = 0;
  currentSavings: number = 0;
  targetSavings: number = 0;
  monthlyContribution: number = 0;
  ageValidate: string="";
  money: string="";

  constructor(private goalService: GoalserviceService, private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.currentAge = Number(params['age']) || 0;
      this.pId = Number(params['pId']) || 0;
    });
  }

  /** ✅ Ensure Valid Inputs */
  validateInputs(): boolean {
    if (this.currentAge >= this.retirementAge) {
      this.ageValidate="Retirement age must be greater than current age.";
      return false;
    }
    else{
      this.ageValidate=" ";
    }
    if (this.currentSavings > this.targetSavings) {
      this.money="Current savings cannot exceed target savings.";
      return false;
    }
    else{
      this.money=" ";
    }
    return true;
  }

  /** ✅ Automatically Calculates Monthly Contribution */
  calculateMonthlyContribution(): void {
    if (this.retirementAge > this.currentAge) {
      const monthsUntilRetirement = (this.retirementAge - this.currentAge) * 12;
      const amountNeeded = this.targetSavings - this.currentSavings;
      this.monthlyContribution = monthsUntilRetirement > 0 ? amountNeeded / monthsUntilRetirement : 0;
    } else {
      this.monthlyContribution = 0; // Default when invalid
    }
  }

  submitGoal(): void {
    if (!this.validateInputs()) {
      return;
    }

    const goalData = {
      pId: this.pId,
      currentAge: this.currentAge,
      retirementAge: this.retirementAge,
      currentSavings: this.currentSavings,
      targetSavings: this.targetSavings,
      monthlyContribution: this.monthlyContribution
    };

    this.goalService.addGoal(goalData).subscribe(
      (storedGoal: any) => {
        alert('Goal Added Successfully!');
        this.goalAdded.emit(storedGoal);
      },
      (error:any) => {
        window.alert("Failed to add goal. Ensure valid values are provided.");
        console.error("Error while storing goal:", error);
      }
    );
  }
}
