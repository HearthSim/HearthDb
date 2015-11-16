mkdir "git_tmp"
git clone --depth=1 "https://github.com/HearthSim/hs-data.git" "git_tmp"
mv "git_tmp/CardDefs.xml" "CardDefs.xml"
rm -r -f "git_tmp"