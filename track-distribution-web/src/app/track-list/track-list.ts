import { Component, inject, OnInit, signal } from '@angular/core';
import { Track } from '../models/track.model';
import { TrackService } from '../services/trackService';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-track-list',
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './track-list.html',
  styleUrl: './track-list.css',
})
export class TrackList implements OnInit {
  private trackService: TrackService;
  private router: Router;
  tracks = signal<Track[]>([]);
  statusFilter = signal('');
  loading = signal(false);
  error = signal('');
  constructor() {
    this.trackService = inject(TrackService);
    this.router = inject(Router);
  }
  ngOnInit(): void {
    this.loadTracks();
  }
  loadTracks(): void {
    this.loading.set(true);
    this.error.set('');
    this.trackService.getTracks(this.statusFilter() ? { status: this.statusFilter() } : {})
      .subscribe({
        next: (data) => { this.tracks.set(data); this.loading.set(false); },
        error: () => { this.error.set('Failed to load tracks.'); this.loading.set(false); }
      });    
  }

  onFilterChange(): void {
    this.loadTracks();
  }
  navigateToTrackDetail(trackId: number): void {
    this.router.navigate(['/tracks', trackId]);
  }
}
