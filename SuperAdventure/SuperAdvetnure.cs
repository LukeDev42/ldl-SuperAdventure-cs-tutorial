using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;

namespace SuperAdventure
{
    
    public partial class SuperAdvetnure : Form
    {
        private Player _player;
        
        public SuperAdvetnure()
        {
            InitializeComponent();

            Location location = new Location(1, "Home", "This is your house.");


            _player = new Player();
            _player.CurrentHitPoints = 10;
            _player.MaximumHitPoints = 10;
            _player.Gold = 20;
            _player.Level = 1;

            lblHitPoint.Text = _player.CurrentHitPoints.ToString();
            lblGold.Text = _player.Gold.ToString();
            lblExperience.Text = _player.ExperiencePoints.ToString();
            lblLevel.Text = _player.Level.ToString();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblExperience_Click(object sender, EventArgs e)
        {

        }

        
    }
}
