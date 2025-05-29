import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { GoalserviceService } from '../../services/goalservice.service';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AddGoalComponent } from '../add-goal/add-goal.component';
import { ContributeComponent } from '../contribute/contribute.component';
import { ProgressComponent } from '../progress/progress.component';
import { AppComponent } from '../../app.component';
@Component({
  selector: 'app-main',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, AddGoalComponent, ContributeComponent, ProgressComponent],
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements AfterViewInit {
  // ✅ User Details
  firstName: string = '';
  lastName: string = '';
  email: string = '';
  gender: string = '';
  age: number = 0;
  pId: number = 0;
  gId: number=0;
  //Goal Tracking
  goals: any = null;
  

  //Modal Management
  modalOpen: boolean = false;
  investmentModalOpen: boolean = false;

 // Savings Tracking
 userSavings: number = 0;
 progressPercentage: number = 0;
//totalSavings:number=0;
//  handleSavingsUpdate(totalSavings: number) {
//   this.totalSavings = totalSavings; // ✅ Update totalSavings when emitted from ProgressComponent
//   console.log("✅ Received Total Savings in MainComponent:", this.totalSavings);
// }

 @ViewChild('progress') progress!: ProgressComponent;
 ngAfterViewInit() {
  console.log("Progress Component Initialized:", this.progress);
}

updateProgress() {
  if (this.progress) {
    this.progress.updateProgress(); // ✅ Call the method dynamically
  } else {
    console.error("Error: Progress component not found.");
  }
}
  constructor(private goalservice: GoalserviceService, private router: Router, private route: ActivatedRoute) {}

  ngOnInit(): void {

    this.route.queryParams.subscribe(params => {
      this.firstName = params['firstName'] || '';
      this.lastName = params['lastName'] || '';
      this.email = params['email'] || '';
      this.gender = params['gender'] || '';
      this.age = Number(params['age']) || 0;
      this.pId = Number(params['pId']) || 0;
      
      

      if (isNaN(this.pId) || this.pId <= 0) {
        console.warn("⚠ Warning: Invalid pId detected. Setting default value.");
        this.pId = 0;
      }

      this.fetchGoals();
      //this.fetchUserProgress();
    });
  }

  logout(): void {
    this.router.navigate(['/']); // Redirect to login page
  }

  fetchGoals(): void {
    this.goalservice.getGoals(this.pId).subscribe(
      (response: any) => {
        console.log("Goals response:", response);
        this.goals = response?.gId ? response : null; //Ensures goals are correctly updated
        
        console.log("Stored Goals in MainComponent:", this.goals);
      },
      (error: any) => {
        console.error("Error fetching goals:", error);
        this.goals = 0; //Prevents null reference issues
      }
    );
  }

  addGoal(): void {
    this.router.navigate(['/add-goal'], { queryParams: { age: this.age, pId: this.pId } });
  }

  contribute(): void {
    if (!this.goals?.gId) {
      console.error("Error: Invalid gId, cannot navigate.");
      return;
    }
    console.log("🔀 Navigating to ContributeComponent with gId:", this.goals.gId);
    // this.totalSavings=this.goals.currentSavings;
    this.router.navigate(['/contribute']);
  }

  // ✅ Modal Controls
  openModal() { this.modalOpen = true; }
  closeModal() { this.modalOpen = false; }
  openInvestmentModal() { this.investmentModalOpen = true; }
  closeInvestmentModal() { this.investmentModalOpen = false; }

  handleGoalAdded(newGoal: any) {
    this.goals = newGoal;
    this.closeModal();
  }

  handleInvestment(response: any) {
    console.log("Investment Submitted Successfully:", response);
    //this.totalSavings=this.goals.currentSavings;
    this.closeInvestmentModal();
  }

  progressModalOpen: boolean = false;

openProgressModal() {
  
  if (!this.goals?.gId) {
    console.error("Error: Cannot fetch savings without a valid goal ID.");
    return;
  }

  this.progressModalOpen = true;
}

closeProgressModal() {
  this.progressModalOpen = false;
}
