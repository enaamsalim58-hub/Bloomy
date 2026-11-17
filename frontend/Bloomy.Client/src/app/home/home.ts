import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {

  categories = [
    { name: 'الهدايا', image: '/Images/h.jpeg' },
    { name: 'الشوكولاتة', image: '/Images/k.jpeg' },
    { name: 'الدمى', image: '/Images/d.jpeg' },
    { name: 'باقات الورود', image: '/Images/f.jpeg' }
  ];

  searchTerm = '';

  scrollToProducts() {
    document.getElementById('products')?.scrollIntoView({
      behavior: 'smooth'
    });
  }

}
