import { Component, Input } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { MainComponent } from './components/main/main.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-root',
  standalone:true,
  imports: [RouterOutlet,CommonModule,FormsModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title='Retirement Planner'

}

