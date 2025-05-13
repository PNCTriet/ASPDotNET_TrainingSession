console.log('Script loaded'); // Kiểm tra script đã được load

document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM Content Loaded'); // Kiểm tra DOM đã load xong

    // Get the password input using a more reliable selector
    const passwordInput = document.querySelector('input[type="password"]');
    const strengthBar = document.getElementById('password-strength-bar');
    const strengthText = document.getElementById('password-strength-text');

    // Debug log để kiểm tra các phần tử
    console.log('Password Input:', passwordInput);
    console.log('Strength Bar:', strengthBar);
    console.log('Strength Text:', strengthText);

    if (passwordInput && strengthBar && strengthText) {
        console.log('All elements found, adding event listener');
        
        passwordInput.addEventListener('input', function() {
            console.log('Password input changed:', this.value);
            const password = this.value;
            const result = zxcvbn(password);
            console.log('Password strength result:', result);
            
            // Update progress bar
            const strength = result.score;
            const width = (strength + 1) * 20; // Convert 0-4 to 25-100%
            strengthBar.style.width = width + '%';
            console.log('Setting width to:', width + '%');

            // Update colors based on strength
            switch(strength) {
                case 0:
                    strengthBar.className = 'progress-bar bg-danger';
                    strengthText.textContent = 'Rất yếu';
                    break;
                case 1:
                    strengthBar.className = 'progress-bar bg-warning';
                    strengthText.textContent = 'Trung Bình';
                    break;
                case 2:
                    strengthBar.className = 'progress-bar bg-info';
                    strengthText.textContent = 'Khá';
                    break;
                case 3:
                    strengthBar.className = 'progress-bar bg-primary';
                    strengthText.textContent = 'Mạnh';
                    break;
                case 4:
                    strengthBar.className = 'progress-bar bg-success';
                    strengthText.textContent = 'Rất mạnh';
                    break;
            }
            console.log('Updated strength to:', strength);

            // Show feedback if available
            if (result.feedback.warning) {
                strengthText.textContent += ' - ' + result.feedback.warning;
                console.log('Added warning:', result.feedback.warning);
            }
        });
    } else {
        console.error('Could not find required elements for password strength meter');
        console.error('Password Input found:', !!passwordInput);
        console.error('Strength Bar found:', !!strengthBar);
        console.error('Strength Text found:', !!strengthText);
    }
});
