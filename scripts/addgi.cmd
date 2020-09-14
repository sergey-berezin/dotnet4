git clone https://github.com/sergey-berezin/%1.git
copy .gitignore %1
cd %1
git add .gitignore
git commit -m "Adding .gitignore for C#"
git push
cd ..