import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { GoalserviceService } from '../../services/goalservice.service';


@Component({
  selector: 'app-progress',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './progress.component.html',
  styleUrl: './progress.component.css'
})
export class ProgressComponent implements OnInit {
  @Input() gId!: number;                 // gId from MainComponent
  @Input() targetSavings!: number;
  @Output() totalSavings = new EventEmitter<any>();  
  userSavings: number = 0;
  progressPercentage: number = 0;
  message:string="";

  constructor(private goalservice: GoalserviceService) {}

  ngOnInit(): void {
    this.fetchUserSavings();
  }


  fetchUserSavings(): void {
    if (!this.gId) {
      console.error("Error: Invalid gId for savings retrieval.");
      return;
    }
  
    this.goalservice.getSavings(this.gId).subscribe(
      (response: any) => {
        console.log("Raw API Response:", response);
  
        // Extract the numeric value (adjust if API structure differs)
        this.userSavings = typeof response === 'object' && response.total_Savings ? response.total_Savings : Number(response);
        console.log("Extracted Savings:", this.userSavings);
        this.totalSavings.emit(this.userSavings); // ✅ Emit updated savings to parent
  
        this.calculateProgress();
      },
      (error) => {
        console.error("Error fetching user savings:", error);
      }
    );
  }
  
  calculateProgress() {
    if (!this.userSavings || !this.targetSavings || isNaN(this.userSavings) || isNaN(this.targetSavings)) {
      console.error("Error: Invalid numbers for progress calculation.");
      this.progressPercentage = 0;              //Prevent NaN% issue
      return;
    }
  
    this.progressPercentage = Number(((this.userSavings / this.targetSavings) * 100).toFixed(2));       //Convert to number
    if(this.progressPercentage>100)
    {
      this.message="Whoa! You're overachieving! While ambition is great, let's make sure you're staying within your planned investment.";
    }
    else if(this.progressPercentage==100)
    {
      this.message="🎉 Hurray!! You've reached your goal!";
    }
    else if (this.progressPercentage >= 75) 
    {
      this.message = "🔥 Almost there! Just a little more to go!";
    }
    else if (this.progressPercentage >= 50) 
    {
      this.message = "💪 Halfway there! Keep pushing forward!";
    }
    else if (this.progressPercentage >= 25)
    {
      this.message = "🚀 You're off to a great start! Keep going!";
    } 
    else {
      this.message = "💡 Every step counts! Keep investing wisely!";
    }
  }
  updateProgress() {
    console.log("Refreshing Progress...");
    this.fetchUserSavings();
  }
  
}

