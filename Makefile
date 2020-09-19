unit: Tests.fs Highlight.fs
	gforth-itc Tests.fs

sample: sample.fs
	gforth-itc Highlight.fs HTML <sample.fs
