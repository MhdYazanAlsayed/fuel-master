// ===========================
// Pricing Toggle
// ===========================
document.addEventListener('DOMContentLoaded', function() {
    const pricingToggle = document.getElementById('pricingToggle');
    
    if (pricingToggle) {
        pricingToggle.addEventListener('change', function() {
            const monthlyPrices = document.querySelectorAll('.monthly-price');
            const yearlyPrices = document.querySelectorAll('.yearly-price');
            
            if (this.checked) {
                // Show yearly prices
                monthlyPrices.forEach(price => price.style.display = 'none');
                yearlyPrices.forEach(price => price.style.display = 'inline');
            } else {
                // Show monthly prices
                monthlyPrices.forEach(price => price.style.display = 'inline');
                yearlyPrices.forEach(price => price.style.display = 'none');
            }
        });
    }
});

// ===========================
// Smooth Scrolling
// ===========================
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        const href = this.getAttribute('href');
        
        // Only prevent default if it's not just "#"
        if (href !== '#' && href !== '#contact') {
            e.preventDefault();
            
            const targetId = href.substring(1);
            const target = document.getElementById(targetId);
            
            if (target) {
                window.scrollTo({
                    top: target.offsetTop - 80,
                    behavior: 'smooth'
                });
            }
        }
    });
});

// ===========================
// Navbar Scroll Effect
// ===========================
window.addEventListener('scroll', function() {
    const navbar = document.querySelector('.navbar');
    
    if (navbar && !navbar.classList.contains('dashboard-navbar')) {
        if (window.scrollY > 50) {
            navbar.style.boxShadow = '0 2px 10px rgba(0,0,0,0.1)';
        } else {
            navbar.style.boxShadow = 'none';
        }
    }
});

// ===========================
// Contact Modal
// ===========================
const contactLink = document.getElementById('contactLink');
if (contactLink) {
    contactLink.addEventListener('click', function(e) {
        e.preventDefault();
        const contactModal = new bootstrap.Modal(document.getElementById('contactModal'));
        contactModal.show();
    });
}

// Contact Form Submission
const contactForm = document.getElementById('contactForm');
if (contactForm) {
    contactForm.addEventListener('submit', function(e) {
        e.preventDefault();
        
        // Show success message
        alert('Thank you for contacting us! We will get back to you soon.');
        
        // Close modal
        const contactModal = bootstrap.Modal.getInstance(document.getElementById('contactModal'));
        if (contactModal) {
            contactModal.hide();
        }
        
        // Reset form
        this.reset();
    });
}

// ===========================
// Dashboard Sidebar Active State
// ===========================
const sidebarItems = document.querySelectorAll('.sidebar-item');
sidebarItems.forEach(item => {
    item.addEventListener('click', function(e) {
        // Don't prevent default for actual navigation
        
        // Remove active class from all items
        sidebarItems.forEach(i => i.classList.remove('active'));
        
        // Add active class to clicked item
        this.classList.add('active');
        
        // Scroll to section
        const targetId = this.getAttribute('href').substring(1);
        const target = document.getElementById(targetId);
        
        if (target) {
            e.preventDefault();
            target.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
    });
});

// ===========================
// Dashboard Functions
// ===========================

// Toggle Password Visibility
function togglePassword(inputId) {
    const input = document.getElementById(inputId);
    const button = event.currentTarget;
    const icon = button.querySelector('i');
    
    if (input.type === 'password') {
        input.type = 'text';
        icon.classList.remove('bi-eye');
        icon.classList.add('bi-eye-slash');
    } else {
        input.type = 'password';
        icon.classList.remove('bi-eye-slash');
        icon.classList.add('bi-eye');
    }
}

// Copy to Clipboard
function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(() => {
        // Show success feedback
        const originalText = event.currentTarget.innerHTML;
        event.currentTarget.innerHTML = '<i class="bi bi-check"></i>';
        event.currentTarget.classList.add('text-success');
        
        setTimeout(() => {
            event.currentTarget.innerHTML = originalText;
            event.currentTarget.classList.remove('text-success');
        }, 2000);
    }).catch(err => {
        console.error('Failed to copy:', err);
        alert('Failed to copy to clipboard');
    });
}

// Show API Key
function showApiKey() {
    const apiKeyElement = event.currentTarget.parentElement.querySelector('code');
    const button = event.currentTarget;
    const icon = button.querySelector('i');
    
    if (apiKeyElement.textContent.includes('••••')) {
        apiKeyElement.textContent = 'sk_live_1234567890abcdefghijklmnop';
        icon.classList.remove('bi-eye');
        icon.classList.add('bi-eye-slash');
    } else {
        apiKeyElement.textContent = 'sk_live_••••••••••••••••';
        icon.classList.remove('bi-eye-slash');
        icon.classList.add('bi-eye');
    }
}

// Create Backup
function createBackup() {
    const button = event.currentTarget;
    const originalText = button.innerHTML;
    
    button.disabled = true;
    button.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Creating backup...';
    
    // Simulate backup creation
    setTimeout(() => {
        button.disabled = false;
        button.innerHTML = originalText;
        alert('Backup created successfully! Download will start automatically.');
        
        // In a real app, this would trigger an actual download
        console.log('Backup download initiated');
    }, 2000);
}

// Schedule Backup
function scheduleBackup() {
    alert('Auto backup scheduling feature coming soon!');
}

// Restore Backup
function restoreBackup() {
    const fileInput = document.getElementById('backupFile');
    
    if (!fileInput.files.length) {
        alert('Please select a backup file first.');
        return;
    }
    
    const confirmed = confirm('Are you sure you want to restore from this backup? This will overwrite current data.');
    
    if (confirmed) {
        const button = event.currentTarget;
        const originalText = button.innerHTML;
        
        button.disabled = true;
        button.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Restoring...';
        
        // Simulate restore process
        setTimeout(() => {
            button.disabled = false;
            button.innerHTML = originalText;
            alert('Backup restored successfully!');
            fileInput.value = '';
        }, 3000);
    }
}

// Root Account Form
const rootAccountForm = document.getElementById('rootAccountForm');
if (rootAccountForm) {
    rootAccountForm.addEventListener('submit', function(e) {
        e.preventDefault();
        
        const button = this.querySelector('button[type="submit"]');
        const originalText = button.innerHTML;
        
        button.disabled = true;
        button.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Creating...';
        
        // Simulate account creation
        setTimeout(() => {
            button.disabled = false;
            button.innerHTML = originalText;
            alert('Root admin user created successfully!');
            this.reset();
        }, 1500);
    });
}

// ===========================
// Intersection Observer for Animations
// ===========================
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -100px 0px'
};

const observer = new IntersectionObserver(function(entries) {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.opacity = '1';
            entry.target.style.transform = 'translateY(0)';
        }
    });
}, observerOptions);

// Observe elements with animation classes
document.querySelectorAll('.fade-in, .slide-up, .feature-card, .use-case-card, .step-card').forEach(el => {
    observer.observe(el);
});

// ===========================
// Console Welcome Message
// ===========================
console.log('%cFuelFlow', 'color: #2563eb; font-size: 24px; font-weight: bold;');
console.log('%cNext-Generation Fuel Management System', 'color: #475569; font-size: 14px;');
console.log('%cBuilt with modern web technologies', 'color: #64748b; font-size: 12px;');
