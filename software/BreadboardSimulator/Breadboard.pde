class BreadBoard
{

	class Row implements Runnable
	{
		int id, x, y, sz;
		boolean on, blinking;
		public final int COLS= 5;
		PFont font;

		Row (int id, int x, int y, int sz)
		{
			this.id= id;
			this.x= x; this.y= y; this.sz= sz; 
			on= false;
			blinking= false; 
			font= loadFont("font.vlw");

			(new Thread(this)).start();
		}

		void draw()
		{
			ellipseMode(CENTER);
			stroke(0);
			textFont(font, 10);
			textAlign(CENTER, CENTER);

			for (int i=0; i<COLS; i++)
			{
				strokeWeight (.5);
				if (on) fill(#90D1F7);
				else fill(#EEEEEE);
				ellipse (x+sz*i, y, sz, sz);

				strokeWeight(1);
				fill(0);
				text(id, x+sz*i, y);
			}
		}

		public void run()
		{
			while (true)
			{
				try
				{
					Thread.sleep(500);
					if (blinking)
					{	
						if ((millis()/1000) % 2 == 0) on=true;
						else on=false;
					}
				}catch (Exception e){}
			}
		}

		void setOn(){ on= true; blinking= false; }
		void setOff(){ on= false; blinking= false; }
		void toggle(){ on= !on; blinking= false; }

		void blink(){ blinking= true; }
		void noblink(){ blinking= false; }
	}


	// Members
	ArrayList<Row> left, right;
	int rowSize;
	public final int ROWS= 16;
	public final int COLS= 5;

	BreadBoard (int rowSize)
	{
		left= new ArrayList<Row> ();
		right= new ArrayList<Row> ();
		this.rowSize= rowSize;

		for (int i=0; i<ROWS; i++)
		{
			Row l= new Row (i+1, 		0, 					i*rowSize, rowSize);
			Row r= new Row (ROWS+i+1, 	0+rowSize*(COLS+2), i*rowSize, rowSize);
			left.add (l);
			right.add (r);
		}
	}

	int getWidth(){return rowSize*(COLS*2+2);}
	int getHeight(){return rowSize*ROWS;}

	void draw()
	{
		int dx= (width-getWidth()+rowSize/2)/2;
		int dy= (height-getHeight()+rowSize)/2;

		pushMatrix();
		translate (dx,dy);
		for (Row r: left) r.draw();
		for (Row r: right) r.draw();
		popMatrix();
	}

	void setOn(int i)
	{
		i--;
		if (i<0 || i>ROWS*2) return;
		if (i>=ROWS) right.get(i%ROWS).setOn();
		else left.get(i).setOn();
	}

	void setOff(int i)
	{
		i--;
		if (i<0 || i>ROWS*2) return;
		if (i>=ROWS) right.get(i%ROWS).setOff();
		else left.get(i).setOff();
	}

	void toggle(int i)
	{
		i--;
		if (i<0 || i>ROWS*2) return;
		if (i>=ROWS) right.get(i%ROWS).toggle();
		else left.get(i).toggle();
	}

	void blink(int i)
	{
		i--;
		if (i<0 || i>ROWS*2) return;
		if (i>=ROWS) right.get(i%ROWS).blink();
		else left.get(i).blink();
	} 

	void noblink(int i)
	{
		i--;
		if (i<0 || i>ROWS*2) return;
		if (i>=ROWS) right.get(i%ROWS).noblink();
		else left.get(i).noblink();
	} 



	void reset()
	{
		for (Row r: left) r.setOff();
		for (Row r: right) r.setOff(); 
	}

	void set(int l, int r, int bl, int br)
	{
		println(l,r,bl,br);
		for (int i=0; i<ROWS; i++)
		{
			// left
			boolean on = ((l >> i) & 1) == 1;
			if (on) setOn(i+1);
			else setOff(i+1);

			// right
			on = ((r >> i) & 1) == 1;
			if (on) setOn(ROWS+i+1);
			else setOff(ROWS+i+1);

			// blink left
			on = ((bl >> i) & 1) == 1;
			if (on) blink(i+1);
			else noblink(i+1);

			// blink right
			on = ((br >> i) & 1) == 1;
			if (on) blink(ROWS+i+1);
			else noblink(ROWS+i+1);
		}


	}

	void set(int l, int r)
	{
		set (l, r, 0, 0);
	}
}





